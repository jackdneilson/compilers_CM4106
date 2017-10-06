using System.Threading.Tasks;
using static System.Math;

namespace lab_2 {
    internal class TeleprompterConfig {
        public bool Done => done;
        private bool done = false;
        
        private object lockHandle = new object();
        public int DelayInMilliseconds { get; private set; } = 200;

        public void UpdateDelay(int increment) {
            int newDelay = Min(DelayInMilliseconds + increment, 1000);
            newDelay = Max(newDelay, 20);
            lock (lockHandle) {
                DelayInMilliseconds = newDelay;
            }
        }

        public void SetDone() {
            done = true;
        }
    }
}