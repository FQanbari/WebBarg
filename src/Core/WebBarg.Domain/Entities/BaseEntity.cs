using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBarg.Domain.Entities;

public interface IEntity;

public abstract class BaseEntity<T> : object, IEntity
{
    public T Id { get; set; }

}
public abstract class BaseEntity : BaseEntity<int>;