import './App.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import Home from './pages/home/page'


const routes = createBrowserRouter([{
  path: '/',
  element: <MainLayout />,
  children: [
    {
      path: '',
      element: <Home />
    }
  ]
}])

export default function App() {
  return (<RouterProvider router={routes} />)
}
