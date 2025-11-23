import React, { useState, useEffect, type ChangeEvent } from 'react';
import axios from 'axios';
import {
    Container, Typography, Button, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow, Paper, IconButton,
    Dialog, DialogTitle, DialogContent, DialogActions, TextField,
    MenuItem, Select, InputLabel, FormControl, Box, type SelectChangeEvent,
    useTheme, useMediaQuery
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';

type AssetType = 'bank' | 'cash' | 'real_estate' | 'inventory';

interface IAsset {
    id: number;
    name: string;
    type: AssetType;
    amount?: number;
    currency?: string;
    bankInfo?: string;
    rawBankName?: string;
    rawAccountNumber?: string;
    estimatedCost?: number;
    initialCost?: number;
    residualCost?: number;
    inventoryNumber?: string;
    quantity?: number;
    unit?: string;
    productionDate?: string;
    address?: string;
    buildYear?: number;
}

interface IAssetFormData {
    name: string;
    amount: number | string;
    currency: string;
    bankName: string;
    accountNumber: string;
    initialCost: number | string;
    residualCost: number | string;
    estimatedCost: number | string;
    inventoryNumber: string;
    quantity: number | string;
    unit: string;
    productionDate: string;
    address: string;
    buildYear: number | string;
}

const API_URL = 'https://localhost:7200/api';

function App() {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

    const [assets, setAssets] = useState<IAsset[]>([]);
    const [open, setOpen] = useState<boolean>(false);
    const [currentAsset, setCurrentAsset] = useState<IAsset | null>(null);

    const [assetType, setAssetType] = useState<AssetType>('bank');

    const [formData, setFormData] = useState<IAssetFormData>({
        name: '',
        amount: 0, currency: 'RUB', bankName: '', accountNumber: '',
        initialCost: 0, residualCost: 0, estimatedCost: 0,
        inventoryNumber: '',
        quantity: 1, unit: 'шт', productionDate: '',
        address: '', buildYear: ''
    });

    const refreshAssets = async () => {
        try {
            const response = await axios.get<IAsset[]>(`${API_URL}/assets`);
            setAssets(response.data);
        } catch (error) {
            console.error("Ошибка при загрузке данных.", error);
        }
    };

    useEffect(() => {
        const load = async () => {
            try {
                const response = await axios.get<IAsset[]>(`${API_URL}/assets`);
                setAssets(response.data);
            } catch (error) {
                console.error("Ошибка загрузки", error);
            }
        };
        load();
    }, []);

    const handleOpen = (asset: IAsset | null = null) => {
        if (asset) {
            setCurrentAsset(asset);
            setAssetType(asset.type);

            setFormData({
                name: asset.name,
                amount: asset.amount || 0,
                currency: asset.currency || 'RUB',
                bankName: asset.rawBankName || '',
                accountNumber: asset.rawAccountNumber || '',
                initialCost: asset.initialCost || 0,
                residualCost: asset.residualCost || 0,
                estimatedCost: asset.estimatedCost || 0,
                inventoryNumber: asset.inventoryNumber || '',
                quantity: asset.quantity || 1,
                unit: asset.unit || 'шт',
                productionDate: asset.productionDate || '',
                address: asset.address || '',
                buildYear: asset.buildYear || ''
            });
        } else {
            setCurrentAsset(null);
            setAssetType('bank');
            setFormData({
                name: '', amount: 0, currency: 'RUB', bankName: '', accountNumber: '',
                initialCost: 0, residualCost: 0, estimatedCost: 0, inventoryNumber: '',
                quantity: 1, unit: 'шт', productionDate: '',
                address: '', buildYear: ''
            });
        }
        setOpen(true);
    };

    const handleClose = () => setOpen(false);

    const handleSave = async () => {
        try {
            const isMonetary = assetType === 'bank' || assetType === 'cash';

            if (isMonetary) {
                const payload = {
                    type: assetType, // "bank" или "cash"
                    name: formData.name,
                    amount: Number(formData.amount),
                    currency: formData.currency,
                    bankName: assetType === 'bank' ? formData.bankName : null,
                    accountNumber: assetType === 'bank' ? formData.accountNumber : null
                };

                if (currentAsset) {
                    await axios.put(`${API_URL}/assets/monetary/${currentAsset.id}`, payload);
                } else {
                    await axios.post(`${API_URL}/assets/monetary`, payload);
                }
            } else {
                const payload = {
                    type: assetType,
                    name: formData.name,
                    initialCost: Number(formData.initialCost),
                    residualCost: Number(formData.residualCost),
                    estimatedCost: Number(formData.estimatedCost),
                    inventoryNumber: formData.inventoryNumber || null,

                    quantity: assetType === 'inventory' ? Number(formData.quantity) : 1,
                    unit: assetType === 'inventory' ? formData.unit : 'шт',
                    productionDate: assetType === 'inventory' && formData.productionDate ? formData.productionDate : null,

                    address: assetType === 'real_estate' ? formData.address : null,
                    buildYear: assetType === 'real_estate' ? Number(formData.buildYear) : null
                };

                if (currentAsset) {
                    await axios.put(`${API_URL}/assets/non-monetary/${currentAsset.id}`, payload);
                } else {
                    await axios.post(`${API_URL}/assets/non-monetary`, payload);
                }
            }
            await refreshAssets();
            handleClose();
        } catch (error) {
            alert("Ошибка сохранения! Проверь консоль.");
            console.error(error);
        }
    };

    const handleDelete = async (id: number) => {
        if (window.confirm("Удалить этот актив?")) {
            try {
                await axios.delete(`${API_URL}/assets/${id}`);
                await refreshAssets();
            } catch (error) {
                console.error("Ошибка удаления", error);
            }
        }
    };

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSelectChange = (e: SelectChangeEvent) => {
        setAssetType(e.target.value as AssetType);
    };

    return (
        <Container maxWidth="md" sx={{ mt: 4, px: isMobile ? 1 : 3 }}>
            <Typography variant="h4" gutterBottom align={isMobile ? 'center' : 'left'}>
                Редактор Активов
            </Typography>

            <Button
                variant="contained"
                fullWidth={isMobile}
                onClick={() => handleOpen(null)}
                sx={{ mb: 2 }}
            >
                Добавить Актив
            </Button>

            <TableContainer component={Paper} sx={{ overflowX: 'auto' }}>
                <Table sx={{ minWidth: 650 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ display: { xs: 'none', sm: 'table-cell' } }}>ID</TableCell>
                            <TableCell>Тип</TableCell>
                            <TableCell>Название</TableCell>
                            <TableCell>Значение</TableCell>
                            <TableCell>Детали</TableCell>
                            <TableCell align="right">Действия</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {assets.map((row) => (
                            <TableRow key={row.id}>
                                <TableCell sx={{ display: { xs: 'none', sm: 'table-cell' } }}>{row.id}</TableCell>
                                <TableCell sx={{ color: 'text.secondary', fontSize: '0.85rem' }}>
                                    {row.type === 'bank' && 'Банк'}
                                    {row.type === 'cash' && 'Касса'}
                                    {row.type === 'real_estate' && 'Недвижимость'}
                                    {row.type === 'inventory' && 'ТМЦ'}
                                </TableCell>
                                <TableCell>{row.name}</TableCell>
                                <TableCell>
                                    {(row.type === 'bank' || row.type === 'cash')
                                        ? <b>{row.amount} {row.currency}</b>
                                        : `Оценка: ${row.estimatedCost}`}
                                </TableCell>
                                <TableCell>
                                    {row.type === 'bank' && row.bankInfo}
                                    {row.type === 'real_estate' && row.address}
                                    {row.type === 'inventory' && `${row.quantity} ${row.unit}`}
                                </TableCell>
                                <TableCell align="right">
                                    <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                                        <IconButton color="primary" onClick={() => handleOpen(row)}>
                                            <EditIcon />
                                        </IconButton>
                                        <IconButton color="error" onClick={() => handleDelete(row.id)}>
                                            <DeleteIcon />
                                        </IconButton>
                                    </Box>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Dialog
                open={open}
                onClose={handleClose}
                maxWidth="sm"
                fullWidth
                fullScreen={isMobile}
            >
                <DialogTitle>
                    {currentAsset ? 'Редактировать актив' : 'Новый актив'}
                </DialogTitle>
                <DialogContent>

                    {!currentAsset && (
                        <FormControl fullWidth margin="normal">
                            <InputLabel>Тип актива</InputLabel>
                            <Select
                                value={assetType}
                                label="Тип актива"
                                onChange={handleSelectChange}
                            >
                                <MenuItem value="bank">Банковский счет</MenuItem>
                                <MenuItem value="cash">Наличные / Талоны</MenuItem>
                                <MenuItem value="inventory">Инвентарь / ТМЦ</MenuItem>
                                <MenuItem value="real_estate">Недвижимость</MenuItem>
                            </Select>
                        </FormControl>
                    )}

                    <TextField
                        label="Название"
                        name="name"
                        fullWidth margin="dense"
                        value={formData.name}
                        onChange={handleChange}
                    />

                    {(assetType === 'bank' || assetType === 'cash') && (
                        <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}>
                            <TextField label="Сумма" name="amount" type="number" fullWidth margin="dense"
                                value={formData.amount} onChange={handleChange} />
                            <TextField label="Валюта" name="currency" fullWidth margin="dense"
                                value={formData.currency} onChange={handleChange} />
                        </Box>
                    )}

                    {assetType === 'bank' && (
                        <>
                            <TextField label="Название Банка" name="bankName" fullWidth margin="dense"
                                value={formData.bankName} onChange={handleChange} />
                            <TextField label="Номер счета" name="accountNumber" fullWidth margin="dense"
                                value={formData.accountNumber} onChange={handleChange} />
                        </>
                    )}

                    {(assetType === 'inventory' || assetType === 'real_estate') && (
                        <>
                            <TextField label="Оценочная стоимость" name="estimatedCost" type="number" fullWidth margin="dense"
                                value={formData.estimatedCost} onChange={handleChange} />

                            <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}>
                                <TextField label="Начальная ст." name="initialCost" type="number" fullWidth margin="dense"
                                    value={formData.initialCost} onChange={handleChange} />
                                <TextField label="Остаточная ст." name="residualCost" type="number" fullWidth margin="dense"
                                    value={formData.residualCost} onChange={handleChange} />
                            </Box>

                            <TextField label="Инвентарный номер" name="inventoryNumber" fullWidth margin="dense"
                                value={formData.inventoryNumber} onChange={handleChange} />
                        </>
                    )}

                    {assetType === 'inventory' && (
                        <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}>
                            <TextField label="Количество" name="quantity" type="number" fullWidth margin="dense"
                                value={formData.quantity} onChange={handleChange} />
                            <TextField label="Ед. измерения" name="unit" fullWidth margin="dense"
                                value={formData.unit} onChange={handleChange} />
                            <TextField label="Дата производства" name="productionDate" type="date" fullWidth margin="dense"
                                InputLabelProps={{ shrink: true }}
                                value={formData.productionDate ? formData.productionDate.split('T')[0] : ''} onChange={handleChange} />
                        </Box>
                    )}

                    {assetType === 'real_estate' && (
                        <>
                            <TextField label="Адрес" name="address" fullWidth multiline rows={2} margin="dense"
                                value={formData.address} onChange={handleChange} />
                            <TextField label="Год постройки" name="buildYear" type="number" fullWidth margin="dense"
                                value={formData.buildYear} onChange={handleChange} />
                        </>
                    )}

                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Отмена</Button>
                    <Button onClick={handleSave} variant="contained">Сохранить</Button>
                </DialogActions>
            </Dialog>
        </Container>
    );
}

export default App;