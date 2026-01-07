import { Navigate, Outlet } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

export default function VeilingmeesterRoute() {
    const token = localStorage.getItem("token");

    if (!token) {
        return <Navigate to="/" replace />;
    }

    try {
        const decoded: any = jwtDecode(token);

        const role =
            decoded["role"] ||
            decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        if (role !== "Veilingmeester") {
            return <Navigate to="/" replace />;
        }

        return <Outlet />;
    } catch (err) {
        console.error("Ongeldige token:", err);
        return <Navigate to="/" replace />;
    }
}
