using System.Collections.Generic;
using System.Linq;

namespace Lumen
{
  public static class MailMerge
  {
    public static string SimpleMerge(string Template, string CommaSeparatedTokens, string CommaSeparatedValues)
    {
      List<string> list1 = Enumerable.ToList<string>((IEnumerable<string>) CommaSeparatedTokens.Split(new char[1]
      {
        ','
      }));
      List<string> list2 = Enumerable.ToList<string>((IEnumerable<string>) CommaSeparatedValues.Split(new char[1]
      {
        ','
      }));
      if (list1.Count != list2.Count)
        return Template;
      string str = Template;
      int count = list1.Count;
      for (int index = 0; index < count; ++index)
        str = str.Replace(list1[index], list2[index]);
      return str;
    }
  }
}
