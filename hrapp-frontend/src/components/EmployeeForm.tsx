import React, { useState, useEffect, ChangeEvent, FormEvent, useCallback } from 'react';
import axiosInstance from '../api/axiosInstance';
import { useForm, Controller, SubmitHandler } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { employeeValidationSchema } from './EmployeeForm.schema';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import InputLabel from '@mui/material/InputLabel';
import FormControl from '@mui/material/FormControl';
import FormHelperText from '@mui/material/FormHelperText';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Alert from '@mui/material/Alert';
import CircularProgress from '@mui/material/CircularProgress';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

interface DepartmentDto { id: string; name: string; }
interface IEmployeeFormInput {
    firstName: string; lastName: string; email: string;
    dateOfBirth: Date | null; hireDate: Date | null;
    position: string | null; departmentId: string | null; salary: number | null;
}
interface Employee {
    id: string; firstName: string; lastName: string; email: string;
    dateOfBirth: string | Date; hireDate: string | Date; position?: string | null;
    departmentId?: string | null; departmentName?: string; salary: number;
}
interface EmployeeFormProps {
    onFormSubmitSuccess: () => void;
    initialData: Employee | null;
    onCancelEdit: () => void;
}

const EmployeeForm: React.FC<EmployeeFormProps> = ({ onFormSubmitSuccess, initialData, onCancelEdit }) => {

    const parseDate = useCallback((dateString: string | Date | null | undefined): Date | null => { /*...*/ }, []); // Önceki kod
    const formatDateForApi = useCallback((date: Date | null): string | null => { /*...*/ }, []); // Önceki kod

    const [departments, setDepartments] = useState<DepartmentDto[]>([]);
    const [serverError, setServerError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const isEditMode = initialData !== null;

    const { control, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<IEmployeeFormInput>({
        resolver: yupResolver(employeeValidationSchema),
        defaultValues: { firstName: '', lastName: '', email: '', dateOfBirth: null, hireDate: null, position: '', departmentId: '', salary: null }
    });

    useEffect(() => { /* fetchDepartments */ }, []);
    useEffect(() => { /* Formu doldurma/resetleme */ }, [initialData, isEditMode, reset, parseDate]);

    const onSubmit: SubmitHandler<IEmployeeFormInput> = async (data) => {
        setServerError(null); setSuccessMessage(null);
        let salaryToSend: number | null = data.salary;
        if (!isEditMode && data.salary === null) { salaryToSend = 0; }

        const employeeDataToSend: any = {
            firstName: data.firstName, lastName: data.lastName, email: data.email,
            dateOfBirth: formatDateForApi(data.dateOfBirth), hireDate: formatDateForApi(data.hireDate),
            position: data.position, departmentId: data.departmentId, salary: salaryToSend
        };
        if (isEditMode && initialData) { employeeDataToSend.id = initialData.id; }

        try {
            let response;
            if (isEditMode && initialData) {
                console.log("Sending PUT:", employeeDataToSend);
                response = await axiosInstance.put(`/employees/${initialData.id}`, employeeDataToSend);
                if (response.status === 204) {
                    setSuccessMessage('Employee updated!'); onFormSubmitSuccess();
                } else { setServerError(`Update failed: ${response.status}`); }
            } else {
                console.log("Sending POST:", employeeDataToSend);
                response = await axiosInstance.post('/employees', employeeDataToSend);
                if (response.status === 201) {
                    setSuccessMessage('Employee added!'); reset(); onFormSubmitSuccess();
                } else { setServerError(`Create failed: ${response.status}`); }
            }
        } catch (err: any) {
             console.error(`Failed: ${err}`);
             let msg = `Error ${err.response?.status || ''}: Failed to ${isEditMode ? 'update' : 'add'} employee.`;
             if (err.response?.data) {
                 if (err.response.data.errors) { msg = `Validation: ${Object.values(err.response.data.errors).flat().join(' ')}`; }
                 else if (err.response.data.message || typeof err.response.data === 'string' || err.response.data.title) { msg = err.response.data.message || err.response.data || err.response.data.title; }
             } else if (err.request) { msg = "Network error..."; } else { msg = `Unexpected error: ${err.message}`; }
             setServerError(msg);
        } finally {
             if (!serverError && successMessage) { setTimeout(() => setSuccessMessage(null), 4000); }
             if (serverError) { setTimeout(() => setServerError(null), 6000); }
        }
    }; // <<<<----- ONSUBMIT BİTTİ ----->>>>

    // <<<<----- RETURN BURADA BAŞLIYOR ----->>>>
    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ p: 3, border: '1px solid #555', borderRadius: 2, maxWidth: 700, margin: '20px auto', bgcolor: '#333' }} noValidate>
                <h2 style={{ color: '#eee', marginBottom: '20px', textAlign: 'center' }}>
                    {isEditMode ? `Edit: ${initialData?.firstName}` : 'Add New Employee'}
                </h2>
                {serverError && <Alert severity="error" sx={{ mb: 2 }} onClose={() => setServerError(null)}>{serverError}</Alert>}
                {successMessage && <Alert severity="success" sx={{ mb: 2 }} onClose={() => setSuccessMessage(null)}>{successMessage}</Alert>}
                <Grid container spacing={2}>
                    {/* Grid items ve Controller'lar - Önceki kodla aynı, sadece value={field.value ?? ''} eklendi */}
                    <Grid item xs={12} sm={6}><Controller name="firstName" control={control} render={({ field }) => <TextField {...field} label="First Name *" variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} error={!!errors.firstName} helperText={errors.firstName?.message} disabled={isSubmitting} />}/></Grid>
                    <Grid item xs={12} sm={6}><Controller name="lastName" control={control} render={({ field }) => <TextField {...field} label="Last Name *" variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} error={!!errors.lastName} helperText={errors.lastName?.message} disabled={isSubmitting} />}/></Grid>
                    <Grid item xs={12}><Controller name="email" control={control} render={({ field }) => <TextField {...field} label="Email *" type="email" variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} error={!!errors.email} helperText={errors.email?.message} disabled={isSubmitting} />}/></Grid>
                    <Grid item xs={12} sm={6}><Controller name="dateOfBirth" control={control} render={({ field }) => <DatePicker {...field} label="Date of Birth *" disabled={isSubmitting} renderInput={(params) => <TextField {...params} variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} sx={{ svg: { color: '#aaa' } }} error={!!errors.dateOfBirth} helperText={errors.dateOfBirth?.message} />} />}/></Grid>
                    <Grid item xs={12} sm={6}><Controller name="hireDate" control={control} render={({ field }) => <DatePicker {...field} label="Hire Date *" disabled={isSubmitting} renderInput={(params) => <TextField {...params} variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} sx={{ svg: { color: '#aaa' } }} error={!!errors.hireDate} helperText={errors.hireDate?.message} />} />}/></Grid>
                    <Grid item xs={12} sm={6}><Controller name="position" control={control} render={({ field }) => <TextField {...field} label="Position" variant="filled" fullWidth InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ style: { color: '#eee', backgroundColor: '#444' } }} error={!!errors.position} helperText={errors.position?.message} disabled={isSubmitting} value={field.value ?? ''}/>}/></Grid>
                    <Grid item xs={12} sm={6}>
                         <FormControl fullWidth variant="filled" disabled={isSubmitting} error={!!errors.departmentId} sx={{ backgroundColor: '#444', borderRadius: 1 }}>
                             <InputLabel sx={{ color: '#aaa' }}>Department</InputLabel>
                             <Controller name="departmentId" control={control} render={({ field }) => ( // defaultValue kaldırıldı
                                 <Select {...field} label="Department" sx={{ color: '#eee', '& .MuiSvgIcon-root': { color: '#aaa' } }} value={field.value ?? ''} >
                                     <MenuItem value=""><em>-- Select --</em></MenuItem>
                                     {departments.map(dep => (<MenuItem key={dep.id} value={dep.id}>{dep.name}</MenuItem>))}
                                 </Select>
                             )}/>
                             {errors.departmentId && <FormHelperText sx={{ color: '#f44336', marginLeft: '14px' }}>{errors.departmentId.message}</FormHelperText>}
                         </FormControl>
                    </Grid>
                    <Grid item xs={12}>
                         <Controller name="salary" control={control} render={({ field: { onChange, value, ...restField } }) => (
                           <TextField {...restField} label="Salary *" type="number" variant="filled" fullWidth required InputLabelProps={{ style: { color: '#aaa' } }} InputProps={{ inputProps: { min: 0, step: "any" }, style: { color: '#eee', backgroundColor: '#444' } }}
                               error={!!errors.salary} helperText={errors.salary?.message} disabled={isSubmitting}
                               onChange={(e) => onChange(e.target.value === '' ? null : parseFloat(e.target.value))}
                               value={value ?? ''}
                           />
                         )}/>
                    </Grid>
                </Grid>
                <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, mt: 3 }}>
                    {isEditMode && ( <Button variant="outlined" color="secondary" onClick={onCancelEdit} disabled={isSubmitting}> Cancel </Button> )}
                    <Button type="submit" variant="contained" color="primary" disabled={isSubmitting} startIcon={isSubmitting ? <CircularProgress size={20} color="inherit"/> : null}>
                        {isEditMode ? 'Update' : 'Add'} Employee
                    </Button>
                </Box>
            </Box>
        </LocalizationProvider>
    );
// --- Component Bitişi ---
};

export default EmployeeForm;