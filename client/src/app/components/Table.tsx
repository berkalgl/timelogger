import moment from 'moment';
import React from 'react';

const useSortableData = (items, config) => {
	const [sortConfig, setSortConfig] = React.useState(config);
  
	const sortedItems = React.useMemo(() => {
	  if (!items)
		return []
	  let sortableItems = [...items];
	  if (sortConfig !== null) {
		sortableItems.sort((a, b) => {
		  if (a[sortConfig.key] < b[sortConfig.key]) {
			return sortConfig.direction === 'ascending' ? -1 : 1;
		  }
		  if (a[sortConfig.key] > b[sortConfig.key]) {
			return sortConfig.direction === 'ascending' ? 1 : -1;
		  }
		  return 0;
		});
	  }
	  return sortableItems;
	}, [items, sortConfig]);
  
	const requestSort = (key) => {
	  let direction = 'ascending';
	  if (
		sortConfig &&
		sortConfig.key === key &&
		sortConfig.direction === 'ascending'
	  ) {
		direction = 'descending';
	  }
	  setSortConfig({ key, direction });
	};
  
	return { items: sortedItems, requestSort, sortConfig };
  };

export default function Table(props) {
	const {columns, data, timesheetColumns} = props;  
	const { items, requestSort, sortConfig } = useSortableData(data, null);
	const getClassNamesFor = (name) => {
	  if (!sortConfig) {
		return;
	  }
	  return sortConfig.key === name ? sortConfig.direction : undefined;
	};

	if (!items && !columns)
	{
		return (<></>)
	}
	return (
		<table className="table-fixed w-full">
			<thead className="bg-gray-200">
				<tr>
					{columns.map((column, i) => {
						if(column.sort)
						{
							return(
								<th key={i} className={column.classes}>
									<button
										type="button"
										onClick={() => requestSort(column.dataField)}
										className={getClassNamesFor(column.dataField)}
										>
										{column.text}
									</button>
								</th>
							)
						}else
						{
							return(
								<th key={i} className={column.classes}>{column.text}</th>
							)
						}
					})}
				</tr>
			</thead>
			<tbody>
                {items.map((row, i) => {
                    return(
						<>
							<tr key = {i} data-toggle="collapse" data-target={"#timesheets" + i} className="accordion-toogle">
								{columns.map((column, j) => {
									return(
										<td key={i + j} className={column.classes}>
											{column.isDate ? moment(row[column.dataField]).format(column.dateFormat) : row[column.dataField]}
										</td>
									)
								})}
							</tr>
							
							<tr>
            					<td colSpan={4} style={{padding: '0 !important'}}>
									<div className="accordian-body collapse" id={"timesheets" + i} style={{marginLeft : '20px', marginRight : '20px'}}> 
										{row['timesheets'] ? <Table columns = {timesheetColumns} data = {row['timesheets']}></Table>: ''}
              						</div> 
          						</td>
							</tr>
						</>
                    )
                })}
			</tbody>
		</table>
	);
}