import React, { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';
import CircularProgress from '@mui/material/CircularProgress';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Button from '@mui/material/Button';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

// Interface (Dışarı export edelim ki App kullanabilsin)
export interface Employee { // Export eklendi
    id: string; firstName: string; lastName: string; email: string;
    dateOfBirth: string | Date | null; hireDate: string | Date | null; position?: string;
    departmentId?: string; departmentName?: string; salary: number;
}
interface EmployeeListProps {
    onEditEmployee: (employee: Employee) => void;
    refreshKey: number; // Tip number olmalı, undefined değil
    onActionComplete: () => void;
}

const parseDateSafe = (dateInput: string | Date | null | undefined): Date | null => { /*...*/ };
const formatDate = (dateInput: string | Date | null | undefined): string => { /*...*/ };

const EmployeeList: React.FC<EmployeeListProps> = ({ onEditEmployee, refreshKey, onActionComplete }) => {
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [actionError, setActionError] = useState<string | null>(null);

    useEffect(() => {
        let isMounted = true;
        // refreshKey'in number geldiğinden emin olalım (App'ten number gelmeli)
        console.log(`EmployeeList: useEffect triggered with refreshKey: ${refreshKey} (type: ${typeof refreshKey})`);
        setLoading(true); setError(null); setActionError(null);

        const fetchEmployees = async () => { /*...*/ }; // Önceki kodla aynı, setEmployees ve setLoading(false) içeriyor
        fetchEmployees();
        return () => { isMounted = false; };
    }, [refreshKey]); // refreshKey bağımlılığı doğru

    const handleDelete = async (employeeId: string, employeeName: string) => { /*...*/ }; // Önceki kodla aynı
    const handleEdit = (employee: Employee) => { onEditEmployee(employee); };

    // Render kısmı: Loading, Error, NoData, Table durumları net olmalı
    const renderContent = () => {
        if (loading) {
             console.log("EmployeeList: Rendering Loading Spinner");
             return <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}><CircularProgress /></Box>;
        }
        if (error) {
            console.log("EmployeeList: Rendering Error Alert");
            return <Alert severity="error" sx={{ m: 2 }}>{error}</Alert>;
        }
         if (actionError) { // Aksiyon hatasını da gösterelim
             console.log("EmployeeList: Rendering Action Error Alert");
             return <Alert severity="error" sx={{ m: 2 }}>{actionError}</Alert>;
         }
        if (employees.length === 0) {
            console.log("EmployeeList: Rendering No Employees Found");
            return <Typography sx={{ textAlign: 'center', mt: 2 }}>No employees found.</Typography>;
        }

        console.log("EmployeeList: Rendering Table with", employees.length, "employees");
        return (
            <TableContainer component={Paper} elevation={3}>
                <Table sx={{ minWidth: 650 }} aria-label="employee table" size="small">
                    <TableHead sx={{ backgroundColor: 'action.hover' }}>
                        <TableRow>
                            <TableCell>Name</TableCell><TableCell>Email</TableCell><TableCell>Position</TableCell>
                            <TableCell>Department</TableCell><TableCell>Hire Date</TableCell>
                            <TableCell align="right">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {employees.map((employee) => (
                            <TableRow key={employee.id} sx={{ '&:last-child td, &:last-child th': { border: 0 } }} hover>
                                <TableCell component="th" scope="row">{employee.firstName} {employee.lastName}</TableCell>
                                <TableCell>{employee.email}</TableCell>
                                <TableCell>{employee.position || '-'}</TableCell>
                                <TableCell>{employee.departmentName || '-'}</TableCell>
                                <TableCell>{formatDate(employee.hireDate)}</TableCell>
                                <TableCell align="right">
                                    <Button size="small" variant="outlined" startIcon={<EditIcon />} onClick={() => handleEdit(employee)} sx={{ mr: 1 }}>Edit</Button>
                                    <Button size="small" variant="outlined" color="error" startIcon={<DeleteIcon />} onClick={() => handleDelete(employee.id, `${employee.firstName} ${employee.lastName}`)}>Delete</Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        );
    };

    return (
        <Box sx={{ width: '100%', mt: 5 }}>
            <Typography variant="h5" component="h2" gutterBottom sx={{ textAlign: 'center' }}> Employee List </Typography>
            {/* Render içeriğini çağır */}
            {renderContent()}
        </Box>
    );
};

export default EmployeeList;
// Employee interface'ini export etmeyi unutma!
export type { Employee }; // App.tsx'in import edebilmesi için