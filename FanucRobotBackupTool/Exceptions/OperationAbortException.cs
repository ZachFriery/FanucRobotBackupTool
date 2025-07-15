
namespace FanucRobotBackupTool.Resources
{
    public class OperationAbortException : Exception
    {
        public OperationAbortException() { }

        public OperationAbortException(string message) : base(message) { }

        public OperationAbortException(string message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return "Operation aborted by user.";
        }
    }
}
