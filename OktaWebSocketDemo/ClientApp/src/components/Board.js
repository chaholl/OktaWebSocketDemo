import React from 'react'
import Cell from './Cell'

const Board = ({ board, onColumnClick, yourTurn }) => {

    if (!board) {
        return <svg width={390} height={355}></svg>
    }

    const cells = []
    const style = { cursor: yourTurn ? 'pointer' : 'no-drop' }
    const numberOfRows = board.length;
    const numberOfColumns = board[0].length;
    const leftMargin = 5;
    const cellSize = 50;

    for (let i = 0; i < numberOfRows; ++i) {
        for (let j = 0; j < numberOfColumns; ++j) {
            const cellKey = i * numberOfColumns + j
            cells.push(<Cell cellKey={cellKey} column={j} row={i} onColumnClick={onColumnClick} style={style} fill={board[i][j]} cellSize={cellSize} leftMargin={leftMargin} />)
        }
    }

    return <svg width={(cellSize * numberOfColumns) + (2 * leftMargin)} height={(cellSize * numberOfRows) + (2 * leftMargin)}>
        {cells}
    </svg>
}


export default Board