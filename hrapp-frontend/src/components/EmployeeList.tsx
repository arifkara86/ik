// src/components/EmployeeList.tsx
import React, { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance';
import Button from '@mui/material/Button'; // MUI Butonları için import

interface Employee {
    id: string; firstName: string; lastName: string; email: string;
    dateOfBirth: string | Date; hireDate: string | Date; position?: string | null;
    departmentId?: string | null; departmentName?: string | null;
    salary: number;
}
interface EmployeeListProps {
    onEditEmployee: (employee: Employee) => void;
    refreshKey: number;
    onActionComplete: () => void;
}

const formatDate = (dateString: string | Date | null | undefined): string => { /*...*/ }; // Önceki kod

const EmployeeList: React.FC<EmployeeListProps> = ({ onEditEmployee, refreshKey, onActionComplete }) => {
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [actionError, setActionError] = useState<string | null>(null);

    useEffect(() => {
        const fetchEmployees = async () => {
            setLoading(true); setError(null); setActionError(null);
            console.log("Fetching employees (refreshKey:", refreshKey, ")");
            try {
                const response = await axiosInstance.get<Employee[]>('/employees');
                setEmployees(response.data);
            } catch (err: any) {
                 console.error("Fetch error:", err);
                 setError("Failed to load data.");
            } finally { setLoading(false); }
        };
        fetchEmployees();
    }, [refreshKey]);

    const handleDelete = async (employeeId: string, employeeName: string) => {
        if (!window.confirm(`Delete ${employeeName}?`)) return;
        setActionError(null);
        try {
            const response = await axiosInstance.delete(`/employees/${employeeId}`);
            if (response.status === 204) {
                console.log(`Delete req sent for ${employeeId}.`);
                // alert(`${employeeName} deleted!`); // Snackbar daha iyi
                onActionComplete(); // App'e bildir
            } else { setActionError(`Delete failed: ${response.status}`); }
        } catch (err: any) {
            console.error("Delete error:", err);
            setActionError(err.response?.data?.message || err.response?.data?.title || "Failed to delete.");
        }
    };

    const handleEdit = (employee: Employee) => {
        console.log("Edit req for:", employee.id);
        onEditEmployee(employee);
    };

    if (loading) return <div style={{ textAlign: 'center', margin: '20px' }}>Loading...</div>;
    if (error) return <Alert severity="error" sx={{ m: 2 }}>{error}</Alert>;

    return (
        <div className="employee-list-container" style={{ marginTop: '30px', color: '#eee' }}>
            <h2>Employee List</h2>
            {actionError && <Alert severity="error" sx={{ mb: 2 }} onClose={() => setActionError(null)}>{actionError}</Alert>}

            {employees.length === 0 ? (<p style={{ color: '#aaa' }}>No employees found.</p>)
            : (
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ borderBottom: '1px solid #555' }}>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Name</th>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Email</th>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Position</th>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Department</th>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Hire Date</th>
                            <th style={{ padding: '10px', textAlign: 'left' }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {employees.map((employee) => (
                            <tr key={employee.id} style={{ borderBottom: '1px solid #444' }}>
                                <td style={{ padding: '10px' }}>{employee.firstName} {employee.lastName}</td>
                                <td style={{ padding: '10px' }}>{employee.email}</td>
                                <td style={{ padding: '10px' }}>{employee.position || '-'}</td>
                                <td style={{ padding: '10px' }}>{employee.departmentName || '-'}</td>
                                <td style={{ padding: '10px' }}>{formatDate(employee.hireDate)}</td>
                                <td style={{ padding: '10px', display: 'flex', gap: '8px' }}>
                                    <Button size="small" variant="outlined" color="warning" onClick={() => handleEdit(employee)}>Edit</Button>
                                    <Button size="small" variant="outlined" color="error" onClick={() => handleDelete(employee.id, `${employee.firstName} ${employee.lastName}`)}>Delete</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default EmployeeList;