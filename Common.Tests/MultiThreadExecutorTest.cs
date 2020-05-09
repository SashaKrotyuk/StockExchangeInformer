namespace Common.Tests
{
	using System.Threading;
	using System.Threading.Tasks;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Threading;

	[TestClass]
	public class MultiThreadExecutorTest : IntegrationTestBase
	{
		private volatile int callsCount;
		private volatile int cancelCount;

		[TestMethod]
		public void MultiThreadExecutorSimultaneouslyTest()
		{
			const int ThreadCount = 30;
			var mte = Resolve<IMultiThreadExecutor>();
			callsCount = 0;

			for (int i = 0; i < ThreadCount; i++)
			{
				mte.AddOperation(new MultiThreadExecutorOperation(TestOperation, callsCount));
			}

			mte.Start();

			WaitMultiThreadExecutorEnd(mte);

			Assert.IsTrue(callsCount == ThreadCount);

			mte.Dispose();
		}

		[TestMethod]
		public void MultiThreadExecutorStopTest()
		{
			const int ThreadCount = 30;
			var mte = Resolve<IMultiThreadExecutor>().Start();
			callsCount = 0;
			cancelCount = 0;

			for (var i = 0; i < ThreadCount; i++)
			{
				var i1 = i;
				var oper = new MultiThreadExecutorOperation(o => { Thread.Sleep(i1 != 0 && i1 % 3 == 0 ? 1200 : 0); TestOperation(null); }, callsCount);
				oper.CancelEvent += OperationCancel;
				mte.AddOperation(oper);

				if (i != 0 && i % 3 == 0)
				{
					var prevCallsCount = callsCount;

					// Wait while added operations start to work.
					while (callsCount == prevCallsCount)
					{
						Thread.Sleep(20);
					}

					// Stop working before all operations complete.
					// Long running operations should be cancelled or aborted.
					mte.Stop();

					Assert.IsTrue(mte.IsStopped());
					Assert.IsTrue(!mte.IsWorking());

					mte.Start();
				}
			}

			WaitMultiThreadExecutorEnd(mte);

			// Some operations on the moment of "cancel" already could be in the running state.
			Assert.IsTrue(cancelCount <= callsCount, "ThreadCount =" + ThreadCount + ", tryCancelCount = " + cancelCount + ", callsCount = " + callsCount);
		}

		private void OperationCancel(object sender, System.EventArgs e)
		{
			cancelCount++;
		}

		private void WaitMultiThreadExecutorEnd(IMultiThreadExecutor mte)
		{
			var task = new Task(() =>
			{
				while (mte.IsWorking())
				{
					// Value is dependent on processor type.
					Thread.Sleep(300);
				}
			});

			task.Start();
			if (!task.Wait(5000))
			{
				Assert.Fail("Multi thread execution has not ended for the expected time");
			}
		}

		private void TestOperation(object state)
		{
			callsCount++;
		}
	}
}
