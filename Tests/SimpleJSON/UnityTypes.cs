using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Tests
{
    public class UnityTypes
    {
        public Vector2 Vector2Field;
        public Vector3 Vector3Field;
        
        public List<Vector2> ListVector2Field;

        public static UnityTypes GenerateRandomValues()
        {
            UnityTypes value = new UnityTypes();
            var random = new Random(0);
            value.Vector2Field = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            value.Vector3Field = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            
            value.ListVector2Field = new List<Vector2>();
            for (int i = 0; i < 3; i++)
            {
                value.ListVector2Field.Add(new Vector2((float)random.NextDouble(), (float)random.NextDouble()));
            }
            
            return value;
        }
    }
}