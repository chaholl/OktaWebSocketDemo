import React from 'react'

const Cell = ({ onColumnClick, cellKey, row, column, fill, style, cellSize, leftMargin }) => {

    const centerOffset = cellSize / 2 - 1;
    const radius = 18;

    return (
        <g key={cellKey} onClick={(_) => onColumnClick(column)} style={style}>
            <rect x={leftMargin + column * cellSize} y={row * cellSize} width={cellSize} height={cellSize} fill={'darkblue'} />
            <circle cx={leftMargin + centerOffset + column * cellSize} cy={centerOffset + row * cellSize} r={radius} fill={fill} />
        </g>)
}


export default Cell