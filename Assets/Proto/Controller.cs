using UnityEngine;

namespace Proto {
    public class Controller : MonoBehaviour {

        public State state;

        protected void SetState(State state) {
            this.state.Exit();
            this.state = state;
            this.state.Enter();
        }
    }
}
