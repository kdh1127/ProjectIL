using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeRabbitPackage.DesignPattern.MVP
{
    public class Model { }
    public class View : MonoBehaviour { }
    public class Presenter : TRSingleton<Presenter> { }
}
