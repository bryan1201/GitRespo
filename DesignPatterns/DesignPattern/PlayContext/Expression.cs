using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayContext
{
    abstract class Expression
    {
        public void Interpret(PlayContext context)
        {
            if (context.PlayText.Length == 0)
                return;

            string playKey = context.PlayText.Substring(0, 1);
            context.PlayText = context.PlayText.Substring(2);
            double playValue = Convert.ToDouble(context.PlayText.Substring(0, context.PlayText.IndexOf(" ")));
            context.PlayText = context.PlayText.Substring(context.PlayText.IndexOf(" ") + 1);

            Excute(playKey, playValue);
        }

        public abstract void Excute(string key, double value);
    }
}
