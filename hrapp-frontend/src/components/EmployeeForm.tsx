import React, { useState, ChangeEvent, FormEvent, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance';
import TextField, { TextFieldProps } from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import InputLabel from '@mui/material/InputLabel';
import FormControl from '@mui/material/FormControl';
import Box from '@mui/material/Box';
import Alert from '@mui/material/Alert';
import CircularProgress from '@mui/material/CircularProgress';
import Typography from '@mui/material/Typography';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

// Interface'ler
interface DepartmentDto { id: string; name: string; }
// Employee tipi dışarıdan (örn: App.tsx'ten) import edilebilir veya burada tanımlanabilir
// import { Employee } from './EmployeeList'; // Eğer EmployeeList export ediyorsa
interface Employee { id: string; firstName: string; lastName: string; email: string; dateOfBirth: string | Date | null; hireDate: string | Date | null; position?: string; departmentId?: string; departmentName?: string; salary: number; }

interface EmployeeFormData {
    firstName: string; lastName: string; email: string;
    dateOfBirth: Date | null; hireDate: Date | null;
    position: string; departmentId: string; salary: number | string;
}
interface EmployeeFormProps {
    onFormSubmitSuccess: () => void;
    initialData: Employee | null;
    onCancelEdit: () => void;
}

// Helper Functions
const parseDate = (dateString: string | Date | null | undefined): Date | null => { /*...*/ };
const formatDateForApi = (date: Date | null): string | null => { /*...*/ };


const EmployeeForm: React.FC<EmployeeFormProps> = ({ onFormSubmitSuccess, initialData, onCancelEdit }) => {
    // States...
    const [departments, setDepartments] = useState<DepartmentDto[]>([]);
    const [formData, setFormData] = useState<EmployeeFormData>({ firstName: '', lastName: '', email: '', dateOfBirth: null, hireDate: null, position: '', departmentId: '', salary: '' });
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const [loadingDepartments, setLoadingDepartments] = useState<boolean>(true);
    const isEditMode = initialData !== null;

    // useEffect'ler...
    useEffect(() => { /* fetchDepartments */ }, []);
    useEffect(() => { /* Formu Doldur/Temizle */ }, [initialData]);

    // Handler'lar...
    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => { /*...*/ };
    const handleSelectChange = (e: SelectChangeEvent<string>) => { /*...*/ };
    const handleDateChange = (name: 'dateOfBirth' | 'hireDate', newValue: Date | null) => { /*...*/ };
    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => { /*...*/ }; // Önceki kod doğru

    // Render
    console.log("EmployeeForm rendering. Edit mode:", isEditMode, "InitialData:", initialData); // Render kontrolü
    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box component="form" onSubmit={handleSubmit} sx={{ '& .MuiTextField-root, & .MuiFormControl-root': { m: 1, width: 'calc(50% - 16px)' }, display: 'flex', flexWrap: 'wrap', p: 3, border: '1px solid', borderColor: 'divider', borderRadius: 2, maxWidth: 700, margin: '20px auto', boxShadow: 3 }}>
                <Typography variant="h5" component="h2" sx={{ width: '100%', textAlign: 'center', mb: 2 }}>
                   {isEditMode ? `Edit: ${initialData?.firstName} ${initialData?.lastName}` : 'Add New Employee'}
                </Typography>

                {error && <Alert severity="error" sx={{ mb: 2, width: '100%' }}>{error}</Alert>}
                {successMessage && <Alert severity="success" sx={{ mb: 2, width: '100%' }}>{successMessage}</Alert>}

                {/* Input Alanları */}
                <TextField label="First Name" name="firstName" value={formData.firstName} onChange={handleInputChange} required disabled={isSubmitting} variant="outlined" size="small"/>
                <TextField label="Last Name" name="lastName" value={formData.lastName} onChange={handleInputChange} required disabled={isSubmitting} variant="outlined" size="small"/>
                <TextField label="Email" name="email" type="email" value={formData.email} onChange={handleInputChange} required disabled={isSubmitting} variant="outlined" size="small" sx={{ width: '100%' }}/>

                {/* DatePickerlar (slots ile) */}
                 <DatePicker
                    label="Date of Birth" value={formData.dateOfBirth} onChange={(newValue) => handleDateChange('dateOfBirth', newValue)} disabled={isSubmitting}
                    slots={{ textField: (params: TextFieldProps) => <TextField {...params} required name="dateOfBirth" variant="outlined" size="small" sx={{width:'100%'}}/> }} // Tam genişlik için sx
                />
                 <DatePicker
                    label="Hire Date" value={formData.hireDate} onChange={(newValue) => handleDateChange('hireDate', newValue)} disabled={isSubmitting}
                    slots={{ textField: (params: TextFieldProps) => <TextField {...params} required name="hireDate" variant="outlined" size="small" sx={{width:'100%'}}/> }} // Tam genişlik için sx
                />

                <TextField label="Position" name="position" value={formData.position} onChange={handleInputChange} disabled={isSubmitting} variant="outlined" size="small"/>
                <FormControl variant="outlined" size="small" disabled={isSubmitting || loadingDepartments} sx={{ m: 1, width: 'calc(50% - 16px)' }}>
                    <InputLabel id="department-select-label">Department</InputLabel>
                    <Select labelId="department-select-label" id="departmentId" name="departmentId" value={formData.departmentId} label="Department" onChange={handleSelectChange} >
                         <MenuItem value="" disabled> <em>{loadingDepartments ? 'Loading...' : '-- Select --'}</em> </MenuItem>
                        {!loadingDepartments && departments.map(dep => (<MenuItem key={dep.id} value={dep.id}>{dep.name}</MenuItem> ))}
                    </Select>
                </FormControl>
                <TextField label="Salary" name="salary" type="number" value={formData.salary} onChange={handleInputChange} required disabled={isSubmitting} variant="outlined" size="small" InputProps={{ inputProps: { min: 0, step: "any" } }}/>

                {/* Butonlar */}
                <Box sx={{ display: 'flex', gap: 1, mt: 2, width: '100%', justifyContent: 'flex-end' }}>
                    {isEditMode && ( <Button variant="outlined" onClick={onCancelEdit} disabled={isSubmitting}> Cancel </Button> )}
                    <Button type="submit" variant="contained" disabled={isSubmitting} startIcon={isSubmitting ? <CircularProgress size={20} color="inherit" /> : null}>
                        {isEditMode ? 'Update Employee' : 'Add Employee'}
                    </Button>
                </Box>
            </Box>
        </LocalizationProvider>
    );
};

export default EmployeeForm;