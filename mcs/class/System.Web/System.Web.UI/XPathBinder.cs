//
// System.Web.UI.XPathBinder
//
// Authors:
//	Ben Maurer (bmaurer@users.sourceforge.net)
//
// (C) 2003 Ben Maurer
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_2_0
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace System.Web.UI {
	public sealed class XPathBinder {
		private XPathBinder ()
		{
		}

		public static object Eval (object container, string xpath)
		{
			return Eval (container, xpath, null);
		}

		public static string Eval (object container, string xpath, string format)
		{
			return Eval (container, xpath, format, null);
		}

		public static string Eval (object container, string xpath, string format, IXmlNamespaceResolver resolver)
		{
			if (xpath == null || xpath.Length == 0)
				throw new ArgumentNullException ("xpath");

			IXPathNavigable factory = container as IXPathNavigable;

			if (factory == null)
				throw new ArgumentException ("container");

			object result = factory.CreateNavigator ().Evaluate (xpath, resolver);

			XPathNodeIterator itr = result as XPathNodeIterator;
			if (itr != null) {
				if (itr.MoveNext ())
					return itr.Current.Value;
				else
					return null;
			}

			if (result == null)
				return String.Empty;
			if (format == null || format.Length == 0)
				return result.ToString ();

			return String.Format (format, result);
		}

		public static IEnumerable Select (object container, string xpath)
		{
			return Select (container, xpath, null);
		}

		public static IEnumerable Select (object container, string xpath, IXmlNamespaceResolver resolver)
		{
			if (xpath == null || xpath.Length == 0)
				throw new ArgumentNullException ("xpath");
			
			IXPathNavigable factory = container as IXPathNavigable;
			
			if (factory == null)
				throw new ArgumentException ("container");
			
			XPathNodeIterator itr = factory.CreateNavigator ().Select (xpath, resolver);
			ArrayList ret = new ArrayList ();
			
			while (itr.MoveNext ()) {
				IHasXmlNode nodeAccessor = itr.Current as IHasXmlNode;
				if (nodeAccessor == null)
					throw new InvalidOperationException ();
				ret.Add (nodeAccessor.GetNode ());
			}
			return ret;
		}
		
	}
}
#endif

