using UnityEngine;

namespace ThreeRabbitPackage.DesignPattern
{
    public class TRBuilder<T> where T : Component
    {
        protected GameObject gameObject;
        protected T component;

        public TRBuilder(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.component = gameObject.GetComponent<T>();

            if (this.component == null)
            {
                this.component = gameObject.AddComponent<T>();
            }
        }
    }
}