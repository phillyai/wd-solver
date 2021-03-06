using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vec = wdsolver.Vector2;

namespace wdsolver
{
    public class Solver {
        private Cell[][] _map;
        private int _truckAmount;
        private bool solved = false;

        public Solver(Cell[][] map, int truckAmount) {
            _map = map;
            _truckAmount = truckAmount;
        }

        public Solver(string map, int truckAmount) : this(Stage.ParseMapFromString(map), truckAmount) {
        }

        public bool TrySolveOneDefaultDirs(out Stage stage, out int step) {
            var dirs0 = new Vec[] { Vec.LEFT, Vec.RIGHT, Vec.UP, Vec.DOWN };
            var dirs1 = new Vec[] { Vec.DOWN, Vec.UP, Vec.RIGHT, Vec.LEFT };
            var tasks = new Task<(bool, int, Stage)>[] { TrySolveOne(dirs0), TrySolveOne(dirs1) };

            var fastest = tasks[Task.WaitAny(tasks)].Result;

            stage = fastest.Item3;
            step = fastest.Item2;
            return fastest.Item1;
        }

        public async Task<(bool, int, Stage)> TrySolveOne(Vec[] dirs) {
            var stage = new Stage(_map, _truckAmount);
            var (b, i) = await TrySolveOne(dirs, stage, 0, 0);
            return (b, i, stage);
        }

        private async Task<(bool, int)> TrySolveOne(Vec[] dirs, Stage stage, int step, int depth) {
            step++;
            if (stage.IsOver()) {
                if (stage.IsWin()) {
                    if (solved)
                        return (true, step);
                    solved = true;
                    return (true, step);
                }
                else return (false, step);
            }

            if (depth % 4 == 0) {
                if (stage.IsNoHope()) {
                    return (false, step);
                }
            }

            foreach (var d in dirs) {
                if (stage.CanGo(in d)) {
                    var actions = stage.Goto(in d);
                    var res = await TrySolveOne(dirs, stage, step, depth + 1);
                    if (res.Item1)
                        return (true, res.Item2);

                    stage.GoBack(actions);
                }
            }
            return (false, step);
        }
    }
}