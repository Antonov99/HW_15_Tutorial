namespace AI.GOAP
{
    public sealed class SubstituteActor : IActor
    {
        public IFactState RequiredState { get; }
        public IFactState ResultState { get; }
        
        public bool IsPlaying
        {
            get { return false; }
        }

        private readonly string id;
        private readonly int cost;
        
        public SubstituteActor(string id, int cost, IFactState requiredState, IFactState resultState)
        {
            this.id = id;
            this.cost = cost;
            RequiredState = requiredState;
            ResultState = resultState;
        }
        
        public int EvaluateCost()
        {
            return cost;
        }

        public bool IsValid()
        {
            return true;
        }

        public void Play(IActor.Callback callback)
        {
        }

        public void Cancel()
        {
        }

        public override string ToString()
        {
            return id;
        }
    }
}