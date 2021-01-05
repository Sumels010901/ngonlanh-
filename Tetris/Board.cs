using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    [Serializable]
    class Board
    {
        #region Variables

        private static int numRows = 18;
        private static int numCols = 10;
        public int[,] grid = new int[numRows,numCols];
        public int boardColor;
        public int rowsCompleted { get; set; }
        public int currentLevel { get; set; }
        public int score { get; set; }
        public Block currentBlock;
        public Coordinate coord;

        #endregion Variables

        public Board()
        {
            this.rowsCompleted = 0;
            this.currentLevel = 1;
            this.score = 0;
            this.currentBlock = new Block();
            this.coord = new Coordinate(0, 0);
            this.colorCodeBoard();
        }
        public bool tick()
        {
            if ( this.currentBlock.currBlock == null || !this.canDrop())
            {
                this.spawnNewBlock();
                return this.isFirstMovePossible();
            }
            this.lowerCurrentBlock();
            this.checkFullRows();
            return true;
        }
        private void spawnNewBlock()
        {
            this.lockLastBlock();
            this.currentBlock.getNextBlock();
        }

        private void maybeUpdateLevel()
        {
            if (this.rowsCompleted % 10 == 0)
                this.currentLevel++;
        }

        public bool isFirstMovePossible()
        {
            if (this.canDrop())
                return true;
            return false;
        }

        public void lowerCurrentBlock()
        {
            if (this.canDrop())
                this.currentBlock.y++;
        }
        private void lockLastBlock()
        {
            if (currentBlock.currBlock != null)
            {
                Coordinate c = null;
                int dim = 4;

                for (int row = 0; row < dim; row++)
                {
                    for (int col = 0; col < dim; col++)
                    {
                        if (currentBlock.currBlock[row, col])
                        {
                            c = currentBlock.toBoardCoord(new Coordinate(col, row));
                            this.grid[c.y, c.x] = currentBlock.blockColor;
                        }
                    }
                }
            }           
        }
        public void moveCurrentBlockLeft()
        {
            if (this.canMoveSideWays(true))
                this.currentBlock.x--;
        }
        public void moveCurrentBlockRight()
        {
            if (this.canMoveSideWays(false))
                this.currentBlock.x++;
        }
        public void rotateCurrentBlockCounterClockwise()
        {
            if (this.canRotate(false))
                this.currentBlock.rotateCounterClockwise();
        }
        public void rotateCurrentBlockClockwise()
        {
            if (this.canRotate(true))
                this.currentBlock.rotateClockwise();
        }
        /// <param name="left"></param>
        /// <returns></returns>
        private bool canMoveSideWays(bool left)
        {
            bool isMoveable = true;
            Block whenMoved = currentBlock.Clone();
            if (left)
                whenMoved.x--;
            else
                whenMoved.x++;

            if (!canBeThere(whenMoved))
                isMoveable = false;

            return isMoveable;
        }
        /// <param name="clockwise"></param>
        /// <returns></returns>
        private bool canRotate(bool clockwise)
        {
            bool isRotatable = true;
            Block whenRotated = currentBlock.Clone();

            if (clockwise)
                whenRotated.rotateClockwise();
            else
                whenRotated.rotateCounterClockwise();

            if (!canBeThere(whenRotated))
                isRotatable = false;

            return isRotatable;
        }
        private bool canDrop()
        {
            bool canDrop = true;
            Block ifDropped = currentBlock.Clone();
            ifDropped.y++;

            if (!canBeThere(ifDropped))
                canDrop = false;
            return canDrop;
        }
        /// <param name="ablock"></param>
        /// <returns></returns>
        private bool canBeThere(Block ablock)
        {
            bool isMoveable = true;
            int dim = 4;

            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    if (ablock.currBlock[row, col])
                    {
                        Coordinate c = ablock.toBoardCoord(new Coordinate(col, row));
                        if (isOccupiedCell(c) || c.x >= numCols || c.x < 0 || c.y >= numRows)
                            isMoveable = false;
                    }
                }
            }
            return isMoveable;
        }
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isOccupiedCell(Coordinate c)
        {
            if (c.x < numCols && c.x >=0 && c.y < numRows && c.y >= 0 && this.grid[c.y, c.x] == this.boardColor)
                return false;
            return true;
        }
        private void checkFullRows()
        {
            int numCompleted = 0;
            int rowPoints = this.currentLevel * 100;
            int rowBonus = this.currentLevel * 50;
            for (int row = 0; row < numRows; row++)
            {
                if (this.isFullRow(row))
                {
                    this.removeRow(row);
                    numCompleted++;
                }             
            }
            if (numCompleted > 0)
            {
                this.score += numCompleted * rowPoints + ((numCompleted - 1) * rowBonus);
                this.updateRowsAndLevel(numCompleted);
            }
        }

        private void updateRowsAndLevel(int numCompleted)
        {
            for (int i = 0; i < numCompleted; i++)
            {
                this.rowsCompleted++;
                if (this.rowsCompleted % 10 == 0)
                    this.currentLevel++;
            }
        }
        /// <param name="currentRow"></param>
        private bool isFullRow(int currentRow)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (this.grid[currentRow, col] == this.boardColor)
                    return false;
            }
            return true;
        }
        /// <param name="removedRow"></param>
        private void removeRow(int removedRow)
        {
            for (int row = removedRow; row > 0; row--)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (row - 1 <= 0)
                        this.grid[row, col] = this.boardColor;
                    else
                        this.grid[row, col] = this.grid[row - 1, col];
                }                
            }
        }
        private void colorCodeBoard()
        {
            this.boardColor = Convert.ToInt32("0xFF000000", 16);
            for (int i = 0; i < numRows; i++ )
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = this.boardColor;
                }
            }
        }
    }
}
