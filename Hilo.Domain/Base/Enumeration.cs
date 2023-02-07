namespace Hilo.Domain.Base;
using System;

public abstract class Enumeration : IComparable
{
    protected Enumeration(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }


    public int Id { get; }
    public string Name { get; }

    public int CompareTo(object? other)
    {
        return other is null
            ? 1
            : other is not Enumeration enumeration
            ? throw new ArgumentException("Object is not an Enumeration")
            : this.Id.CompareTo(enumeration.Id);
    }
}