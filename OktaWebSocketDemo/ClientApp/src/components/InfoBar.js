import React from 'react'

const InfoBar = ({ message, color }) => {
    let style = { color: color};
    return <p className="lead text-center text-capitalize" style={style}>{message}</p>    
}


export default InfoBar