import * as yup from 'yup';
export const employeeValidationSchema = yup.object().shape({
    firstName: yup.string().required('First Name is required').max(100),
    lastName: yup.string().required('Last Name is required').max(100),
    email: yup.string().required('Email is required').email('Enter a valid email').max(150),
    dateOfBirth: yup.date().required('Date of Birth is required').typeError('Invalid date').max(new Date()).nullable(),
    hireDate: yup.date().required('Hire Date is required').typeError('Invalid date').nullable(),
    position: yup.string().max(100).nullable().transform(v => v === "" ? null : v),
    departmentId: yup.string().nullable().transform(v => v === "" ? null : v),
    salary: yup.number().required('Salary is required').typeError('Salary must be a number').min(0).nullable()
        .transform((v, ov) => String(ov).trim() === "" || isNaN(v) ? null : v),
});