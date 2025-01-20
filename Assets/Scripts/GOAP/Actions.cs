using System.Collections.Generic;

namespace GOAP
{
    public static class Actions
    {
        public static List<Action> GetActions()
        {
            List<Action> actions = new List<Action>();

            actions.Add(new Action("perambular",
                new State(new List<(string, object)> {
                    new("temAlvo", false),
                    new("eFraco", false)
                }),
                new State(new List<(string, object)>
                {
                    new("temAlvo", true)
                }),
                1
                ));

            actions.Add(new Action("fugir",
                new State(new List<(string, object)> {
                    new("sendoAtacado", true),
                    new("eFraco", true)
                }),
                new State(new List<(string, object)>
                {
                    new("sendoAtacado", false)
                }),
                1
                ));


            return actions;
        }
    }
}

