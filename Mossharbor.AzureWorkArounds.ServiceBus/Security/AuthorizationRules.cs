using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [Serializable]
    public class AuthorizationRules : ICollection<AuthorizationRule>, IEnumerable<AuthorizationRule>, IEnumerable, IXmlSerializable
    {
        [XmlIgnore]
        public readonly ICollection<Mossharbor.AzureWorkArounds.ServiceBus.AuthorizationRule> innerCollection;
        [XmlIgnore]
        private readonly IDictionary<string, Mossharbor.AzureWorkArounds.ServiceBus.SharedAccessAuthorizationRule> nameToSharedAccessAuthorizationRuleMap;

        public AuthorizationRules()
        {
			this.nameToSharedAccessAuthorizationRuleMap = new Dictionary<string, SharedAccessAuthorizationRule>(StringComparer.OrdinalIgnoreCase);
			this.innerCollection = new List<AuthorizationRule>();
        }

        /// <summary>Initializes a new instance of the 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> class with a list of 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" />.</summary> 
		/// <param name="enumerable">The list of <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" />.</param>
		public AuthorizationRules(IEnumerable<Mossharbor.AzureWorkArounds.ServiceBus.AuthorizationRule> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            this.nameToSharedAccessAuthorizationRuleMap = new Dictionary<string, SharedAccessAuthorizationRule>(StringComparer.OrdinalIgnoreCase);
            this.innerCollection = new List<AuthorizationRule>();
            foreach (AuthorizationRule authorizationRule in enumerable)
            {
                this.Add(authorizationRule);
            }
        }

        /// <summary>Adds the specified <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> into the collection.</summary>
		/// <param name="item">The <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> to be added.</param>
		public void Add(Mossharbor.AzureWorkArounds.ServiceBus.AuthorizationRule item)
        {
            SharedAccessAuthorizationRule sharedAccessAuthorizationRule;
            if (item is SharedAccessAuthorizationRule)
            {
                SharedAccessAuthorizationRule sharedAccessAuthorizationRule1 = item as SharedAccessAuthorizationRule;
                if (this.nameToSharedAccessAuthorizationRuleMap.TryGetValue(sharedAccessAuthorizationRule1.KeyName, out sharedAccessAuthorizationRule))
                {
                    this.nameToSharedAccessAuthorizationRuleMap.Remove(sharedAccessAuthorizationRule1.KeyName);
                    this.innerCollection.Remove(sharedAccessAuthorizationRule);
                }
                this.nameToSharedAccessAuthorizationRuleMap.Add(sharedAccessAuthorizationRule1.KeyName, sharedAccessAuthorizationRule1);
            }
            this.innerCollection.Add(item);
        }

        /// <summary>Clears all elements in the collection.</summary>
		public void Clear()
        {
            this.nameToSharedAccessAuthorizationRuleMap.Clear();
            this.innerCollection.Clear();
        }

        /// <summary>Determines whether the specified item exists in the collection.</summary>
		/// <param name="item">The item to search in the collection.</param>
		/// <returns>true if the specified item is found; otherwise, false.</returns>
		public bool Contains(Mossharbor.AzureWorkArounds.ServiceBus.AuthorizationRule item)
        {
            return this.innerCollection.Contains(item);
        }

        /// <summary>Copies the elements into an array starting at the specified array index.</summary>
		/// <param name="array">The array to hold the copied elements.</param>
		/// <param name="arrayIndex">The zero-based index at which copying starts.</param>
		public void CopyTo(AuthorizationRule[] array, int arrayIndex)
        {
            this.innerCollection.CopyTo(array, arrayIndex);
        }
        
        /// <summary>Gets the enumerator that iterates through the collection.</summary>
		/// <returns>The enumerator that can be used to iterate through the collection.</returns>
		public IEnumerator<Mossharbor.AzureWorkArounds.ServiceBus.AuthorizationRule> GetEnumerator()
        {
            return this.innerCollection.GetEnumerator();
        }

        /// <summary>Gets the sets of <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" />.</summary>
		/// <param name="match">The authorization rule to match the specified value.</param>
		/// <returns>The sets of <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> that match the specified value.</returns>
		public List<AuthorizationRule> GetRules(Predicate<AuthorizationRule> match)
        {
            return ((List<AuthorizationRule>)this.innerCollection).FindAll(match);
        }

        /// <summary>Gets the set of <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> that matches the specified value.</summary>
        /// <param name="claimValue">The value to search for.</param>
        /// <returns>The sets of <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> that match the specified value.</returns>
        public List<AuthorizationRule> GetRules(string claimValue)
        {
            return ((List<AuthorizationRule>)this.innerCollection).FindAll((AuthorizationRule rule) => string.Equals(claimValue, rule.ClaimValue, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Removes the specified <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRule" /> from the collection.</summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>true if the operation succeeded; otherwise, false.</returns>
		public bool Remove(AuthorizationRule item)
        {
            return this.innerCollection.Remove(item);
        }

        /// <summary>Gets the enumerator that iterates through the collection.</summary>
		/// <returns>The enumerator that can be used to iterate through the collection.</returns>
		IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerCollection.GetEnumerator();
        }

        /// <summary>Gets the rule associated with the specified key.</summary>
		/// <param name="keyName">The name of the key.</param>
		/// <param name="rule">The rule associated with the specified key.</param>
		/// <returns>true if the 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> contains an element with the specified key; otherwise, false.</returns> 
		public bool TryGetSharedAccessAuthorizationRule(string keyName, out SharedAccessAuthorizationRule rule)
        {
            return this.nameToSharedAccessAuthorizationRuleMap.TryGetValue(keyName, out rule);
        }
        
        public int Count
        {
            get
            {
                return this.innerCollection.Count;
            }
        }

        [XmlIgnore]
        /// <summary>Gets or sets whether the <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> is read only.</summary>
		/// <value>true if the <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> is read only; otherwise, false.</value>
		public bool IsReadOnly
        {
            get
            {
                return this.innerCollection.IsReadOnly;
            }
        }

        [XmlIgnore]
        /// <summary>Gets a value that indicates whether the <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> requires encryption.</summary>
		/// <value>true if the <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" /> requires encryption; otherwise, false.</value>
		public bool RequiresEncryption
        {
            get
            {
                return this.nameToSharedAccessAuthorizationRuleMap.Any<KeyValuePair<string, SharedAccessAuthorizationRule>>();
            }
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            IEnumerable<XElement> authRules = RuleDescription.SimpleStreamAxis(reader, new string[] { "AuthorizationRule" }).ToArray();

            foreach (var authRule in authRules)
            {
                var actionTypeAttrib = authRule?.Attributes()?.FirstOrDefault(p => p.Name.LocalName.Equals("type"));
                switch (actionTypeAttrib?.Value?.ToLower())
                {
                    case "sharedaccessauthorizationrule":
                        //var expression = authRule?.Elements(XName.Get("{http://schemas.microsoft.com/netservices/2010/10/servicebus/connect}AuthorizationRule"));
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(SharedAccessAuthorizationRule));
                        var stringReader = new StringReader(authRule.ToString());
                        SharedAccessAuthorizationRule rule =  (SharedAccessAuthorizationRule)xs.Deserialize(stringReader);
                        this.Add(rule);
                        break;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (AuthorizationRule rule in this)
            {
                XmlSerializer ruleSerlizer = new XmlSerializer(rule.GetType(), "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect"); //, 
                ruleSerlizer.Serialize(writer, rule);
            }
        }
    }
}
