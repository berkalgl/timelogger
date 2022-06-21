import React from 'react';
import { configure, shallow } from 'enzyme';
import Projects from './Projects';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });

describe('<Projects />', () => {

    const wrapper = shallow((
        <Projects/>
    ));
    console.log(wrapper.debug())
    it('should have two buttons', () => {
        expect(wrapper.find('button').length).toEqual(2);
    });
    
    it('should have Table', () => {
        expect(wrapper.find('Table').length).toEqual(1);
    });
    it('should have renders and add Form', () => {
        expect(wrapper.find('ProjectAddForm').length).toEqual(1);
    });

    it('should have set addForm open to true', () => {
        expect(wrapper.find('ProjectAddForm').prop('showModal')).toBeFalsy();
        wrapper.find('button#projectAddBtn').simulate('click');
        expect(wrapper.find('ProjectAddForm').prop('showModal')).toBeTruthy()
    });
})

