import React from 'react';
import { configure, shallow } from 'enzyme';
import Modal from './Modal';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });

describe('<Modal />', () => {
    const wrapper = shallow((
        <Modal
            show={true}
            title="Title"
        >
            Content
        </Modal>
    ));

    it('should have open', () => {
        expect(wrapper.find('Modal').prop('show')).toBe(true);
    });

    it('should have Modal Body Content', () => {
        expect(wrapper.find('ModalBody').prop('children')).toBe('Content');
    });
    
    it('should have Modal Title Title', () => {
        expect(wrapper.find('ModalTitle').prop('children')).toBe('Title');
    });

    it('should have header', () => {
        expect(wrapper.find('ModalHeader').length).toEqual(1);
    });
    it('should have body', () => {
        expect(wrapper.find('ModalBody').length).toEqual(1);
    });
    it('should have footer', () => {
        expect(wrapper.find('ModalFooter').length).toEqual(1);
    });
    it('should contain two buttons', () => {
        expect(wrapper.find('Button').length).toEqual(2);
    });
})

