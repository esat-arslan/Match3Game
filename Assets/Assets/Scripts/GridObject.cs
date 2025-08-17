using UnityEngine;

public class GridObject<T> {
        GridSystem2D<GridObject<T>> grid;
        int x;
        int y;
        T gem;
        
        public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(T gem) {
            this.gem = gem;
        }
        
        public T GetValue() => gem;

        public void UpdatePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
