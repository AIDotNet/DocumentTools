import { Outlet } from "react-router-dom";
import { CloseWindow } from "../services/chatWindowService";
import { Button } from "antd";
import { CloseOutlined } from "@ant-design/icons";

export default function MainLayout() {
  return (
    <>
      <Button style={{
        position: "absolute",
        top: 0,
        right: 0,
        zIndex: 1000,

      }} 
      icon={<CloseOutlined />}
      type='text'
      onClick={() => CloseWindow()}></Button>
      <Outlet />
    </>
  )
}