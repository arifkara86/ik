import React, { useState, useCallback, createContext, useContext } from 'react';
import EmployeeList from './components/EmployeeList';
import EmployeeForm from './components/EmployeeForm';
import './App.css';
import Snackbar from '@mui/material/Snackbar';
import Alert, { AlertColor } from '@mui/material/Alert';

interface Employee { /*...*/ }
interface SnackbarContextType { showSnackbar: (message: string, severity?: AlertColor) => void; }
const SnackbarContext = createContext<SnackbarContextType | null>(null);
export const useSnackbar = () => { /*...*/ };

function App() { /* State ve Fonksiyonlar önceki gibi */
    const [listKey, setListKey] = useState<number>(0);
    const [employeeToEdit, setEmployeeToEdit] = useState<Employee | null>(null);
    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMessage, setSnackbarMessage] = useState('');
    const [snackbarSeverity, setSnackbarSeverity] = useState<AlertColor>('success');
    const showSnackbar = useCallback((message: string, severity: AlertColor = 'success') => { /*...*/ }, []);
    const handleSnackbarClose = (event?: React.SyntheticEvent | Event, reason?: string) => { /*...*/ };
    const refreshList = useCallback(() => { /*...*/ }, [employeeToEdit, showSnackbar]);
    const handleEditEmployee = useCallback((employee: Employee) => { /*...*/ }, []);
    const handleCancelEdit = useCallback(() => { setEmployeeToEdit(null); }, []);

    return (
        <SnackbarContext.Provider value={{ showSnackbar }}>
            <div className="App">
                <header className="App-header"> <h1>HR Management Portal</h1> </header>
                <main>
                    <EmployeeForm /* Props önceki gibi */ />
                    <EmployeeList /* Props önceki gibi */ />
                </main>
                <Snackbar /* Props önceki gibi */ >
                    <Alert /* Props önceki gibi */ >{snackbarMessage}</Alert>
                </Snackbar>
            </div>
        </SnackbarContext.Provider>
    );
}
export default App;