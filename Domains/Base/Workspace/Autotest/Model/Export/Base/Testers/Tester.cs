using System;
using System.Collections.Generic;
using Autotest.Html;

namespace Autotest.Testers
{
    public abstract partial class Tester
    {
        public Element Element { get; }
        
        protected Tester(Element element)
        {
            this.Element = element;
        }
        
        public bool this[string typeCheck] => string.Equals($"is{this.Element.Name}", typeCheck, StringComparison.OrdinalIgnoreCase) || 
                                              string.Equals($"is{this.GetType().Name}", typeCheck, StringComparison.OrdinalIgnoreCase);
    }
}