using System;
using MongoDB.Bson;

namespace Common.Types
{
    public interface IIdentifiable
    {
        string Id { get; }
    }
}