namespace TreeEdit
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal static class Validation
    {
        public static void ValidateAny<T>(IEnumerable<T> items, int from = 0, string type = "number", string action = "")
        {
            if (0 <= from && from < items.Count())
            {
                return;
            }

            action = action == "" ? action : " to " + action;

            string message = $"Please specify a {type}{action}!";

            throw new ApplicationException(message);
        }

        public static void ValidateTreeNonEmpty()
        {
            if (!State.Tree.Any())
            {
                throw new ApplicationException("Tree is empty!");
            }
        }
    }
}