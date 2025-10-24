import { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";

export default function Dashboard() {
  const [data, setData] = useState<{ linea: string; cantidad: number }[]>([]);
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    const start = async () => {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_API_BASE_URL}/lineshub`)
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

      connectionRef.current = connection;

      connection.on("ReceiveUpdate", (message: string) => {
      try {
        const parsed = JSON.parse(message);
        setData((prev) => [
          ...prev,
          { linea: parsed.LineId.toString(), cantidad: parsed.ProductionCount }
        ]);
      } catch (e) {
        console.error("Error parsing message:", message, e);
      }
    });

      try {
        await connection.start();
        console.log("Conectado a SignalR");
      } catch (err) {
        console.error("SignalR start failed:", err);
      }
    };

    start();

    return () => {
      const conn = connectionRef.current;
      if (conn) {
        conn.stop().catch((err) => console.error("Error stopping SignalR connection:", err));
      }
    };
  }, []);

  return (
    <div>
      <h1>Producción por Línea</h1>
      {data.map((d, i) => (
        <p key={i}>{d.linea}: {d.cantidad}</p>
      ))}
    </div>
  );
}
