using System;

namespace GameCtor.DevToolbox
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
    }

    public interface IVisitor
    {
        void Visit(UnityEngine.Object obj);
    }

    public interface IMonoInject
    {
        void Accept(PrefabFactory1 visitor);
    }
}
