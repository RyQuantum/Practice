import React, { useState, useEffect } from 'react'

const usePerson = (name) => {
const [loading, setLoading] = useState(false)
const [person, setPerson] = useState({})
  console.log("ryan")
  // setLoading(true)
  // setTimeout(()=> {
  //   setLoading(false)
  //   setPerson({name})
  // },2000)
  useEffect(() => {
    setLoading(true)
    setTimeout(()=> {
      setLoading(false)
      setPerson({name})
    },2000)
  },[name])
  return [loading,person]
}

const AsyncPage = ({name}) => {
  const [loading, person] = usePerson(name)
    return (
      <>
        {loading?<p>Loading...</p>:<p>{person.name}</p>}
      </>
    )
  }

const PersonPage = () =>{
  const [state, setState]=useState('')
  const changeName = (name) => {
    setState(name)
  }
  return (
    <>
      <AsyncPage name={state}/>
      <button onClick={() => {changeName('名字1')}}>名字1</button>
      <button onClick={() => {changeName('名字2')}}>名字2</button>
    </>
  )
}

export default PersonPage 