namespace Web.Services.Helpers
{
    using Web.Models;

    public readonly struct Result<A>
    {
        public readonly ResultStateEnum State;
        public readonly A? Value;
        public readonly ResultError? resulterror;

        internal ResultError ResultError => resulterror ?? new ResultError("An error occured");

        public Result(A value, bool isError = false)
        {
            State = isError ? ResultStateEnum.Faulted : ResultStateEnum.Success;
            Value = value;
            resulterror = null;
        }

        public Result(ResultError e)
        {
            State = ResultStateEnum.Faulted;
            resulterror = e;
            Value = default;
        }

        public static implicit operator Result<A>(A value) => new(value);
    }
}
