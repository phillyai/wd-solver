﻿using System.Collections.Generic;
using System.Linq;

namespace wdsolver {
    public class Graph {
        List<int>[] adj;
        int width, height;
        public Graph(int width, int height) {
            var len = width * height + 1;
            adj = new List<int>[len];
            for (int i = 0; i < len; i++)
                adj[i] = new List<int>(capacity: 4);

            this.width = width;
            this.height = height;
        }

        public void Clear() {
            foreach (var i in adj) {
                i.Clear();
            }
        }

        public void Add(int s, int d) {
            adj[s].Add(d);
            adj[d].Add(s);
        }

        public bool BFS(int s, int d, bool nearD) {
            if (s == d)
                return true;

            var visited = new bool[adj.Length];
            for (int i = 0; i < visited.Length; i++)
                visited[i] = false;

            visited[s] = true;
            var queue = new Queue<int>();
            queue.Enqueue(s);

            var nearDs = new[] { d - 1, d + 1, d - width, d + width };

            while (queue.Count > 0) {
                s = queue.Dequeue();
                foreach (var i in adj[s]) {
                    if (nearD) {
                        if (nearDs.Contains(i))
                            return true;
                    } else if (i == d)
                        return true;

                    if (!visited[i]) {
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }
            return false;
        }

        public bool BFS(int sx, int sy, int dx, int dy, bool nearD) {
            int s = sx + sy * width;
            int d = dx + dy * width;

            return BFS(s, d, nearD);
        }

        public bool BFSFromNearSources(int s, int d, bool nearD) {
            var nearSs = new[] { s - 1, s + 1, s - width, s + width }.Where(i => i >= 0);

            var queue = new Queue<int>();
            var visited = new bool[adj.Length];
            for (int i = 0; i < visited.Length; i++)
                visited[i] = false;

            RegisterSourceNear(s - 1);
            RegisterSourceNear(s + 1);
            RegisterSourceNear(s - width);
            RegisterSourceNear(s + width);

            var nearDs = new[] { d - 1, d + 1, d - width, d + width };

            while (queue.Count > 0) {
                s = queue.Dequeue();
                foreach (var i in adj[s]) {
                    if (nearD) {
                        if (nearDs.Contains(i))
                            return true;
                    } else if (i == d)
                        return true;

                    if (!visited[i]) {
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }
            return false;

            void RegisterSourceNear(int i) {
                if (i >= 0) {
                    visited[i] = true;
                    queue.Enqueue(i);
                }
            }
        }

        public bool BFSFromNearSources(int sx, int sy, int dx, int dy, bool nearD) {
            int s = sx + sy * width;
            int d = dx + dy * width;

            return BFSFromNearSources(s, d, nearD);
        }
    }
}
