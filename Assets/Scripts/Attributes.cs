using System.ComponentModel;
using UnityEngine;

public class MyConfigAttribute : PropertyAttribute { }

// TODO: Move somewhere else because this is not an attribute
namespace System.Runtime.CompilerServices
{
    public record IsExternalInit;
}
