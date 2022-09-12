using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDB_Reproduction.Models {
    public class TestEntity {
        public string Id {
            get; set;
        }
        public string Name {
            get; set;
        }
        public int Category {
            get; set;
        }
        public int Quantity {
            get; set;
        }
        public TestEntity(string id, string name, int category, int quantity, bool sale) {
            this.Id = id;
            this.Name = name;
            this.Category = category;
            this.Quantity = quantity;
        }

        public enum Categories {
            Zero,One,Two,Three,Four,Five,Six,Seven
        }
    }
}
