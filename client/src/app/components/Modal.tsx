import React from 'react';
import { Button, Modal } from 'react-bootstrap';

export default function CommonModal(props) {
	
	const {show, handleClose, handleSave, title} = props;  

	if (!show)
		return (<></>)
		
	return (
		<Modal show={show} onHide={handleClose}>
			<Modal.Header closeButton>
				<Modal.Title>{title}</Modal.Title>
			</Modal.Header>
			<Modal.Body>{props.children}</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={handleClose}>
					Close
				</Button>
				<Button variant="primary" onClick={handleSave}>
					Save Changes
				</Button>
			</Modal.Footer>
		</Modal>
	);
}