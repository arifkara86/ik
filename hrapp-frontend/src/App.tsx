// src/App.tsx
import React, { useState, useCallback } from 'react';
import EmployeeList from './components/EmployeeList';
import EmployeeForm from './components/EmployeeForm'; // Form import ediliyor mu?
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import './App.css'; // Opsiyonel

// Employee interface
interface Employee {
    id: string; firstName: string; lastName: string; email: string;
    dateOfBirth: string | Date | null; hireDate: string | Date | null; position?: string;
    departmentId?: string; departmentName?: string; salary: number;
}

function App() {
    console.log("App component rendering or re-rendering");
    const [listKey, setListKey] = useState<number>(Date.now());
    const [employeeToEdit, setEmployeeToEdit] = useState<Employee | null>(null);

    const refreshListAndClearForm = useCallback(() => {
        console.log("App: refreshListAndClearForm triggered");
        setEmployeeToEdit(null);
        setListKey(Date.now());
    }, []);

    const handleEditEmployee = useCallback((employee: Employee) => {
        console.log("App: handleEditEmployee triggered for:", employee.id);
        setEmployeeToEdit(employee);
    }, []);

    const handleCancelEdit = useCallback(() => {
        console.log("App: handleCancelEdit triggered");
        setEmployeeToEdit(null);
    }, []);

    return (
        <>
            <CssBaseline />
            <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
                <Box sx={{ textAlign: 'center', mb: 4 }}>
                    <Typography variant="h4" component="h1" gutterBottom>
                        HR Management Portal
                    </Typography>
                </Box>

                <main>
                    {/* --- FORM RENDER KISMI --- */}
                    {/* Bu kısım kesinlikle olmalı */}
                    {console.log("App: Rendering EmployeeForm")}
                    <EmployeeForm
                        key={employeeToEdit ? `edit-${employeeToEdit.id}` : 'add-new-employee-form'} // Key'i daha belirgin yapalım
                        onFormSubmitSuccess={refreshListAndClearForm}
                        initialData={employeeToEdit}
                        onCancelEdit={handleCancelEdit}
                    />
                    {/* --- FORM RENDER KISMI BİTİŞ --- */}


                    <Box sx={{ mt: 5 }}>
                        {console.log("App: Rendering EmployeeList")}
                        <EmployeeList
                            refreshKey={listKey}
                            onEditEmployee={handleEditEmployee}
                            onActionComplete={refreshListAndClearForm}
                        />
                    </Box>
                </main>
            </Container>
        </>
    );
}

export default App;