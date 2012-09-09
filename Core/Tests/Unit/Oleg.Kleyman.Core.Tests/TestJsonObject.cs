using System;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Core.Tests
{
    [Serializable]
    public class TestJsonObject
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}