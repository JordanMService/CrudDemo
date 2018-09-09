import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import AddModal from './Modals/AddModal';
import DeleteModal from './Modals/DeleteModal';
import DisplayModal from './Modals/DisplayModal';
import ItemService from "../Services/ItemService";
import { Button } from 'semantic-ui-react';

export class NavMenu extends React.Component<{}, {}> {

    private renderModalButton = (buttonText: string,clickFn: (e : React.SyntheticEvent<any>) => void) =>{
            return(<a onClick={clickFn}>{buttonText}</a>)
    } 

    public render() {
        return (
                <div className='navbar navbar-default navbar-fixed-top'>
                <div className='navbar-header'>
                    <span className="navbar-brand">Demo App</span>
                    <button type='button' className='navbar-toggle' data-toggle='collapse' data-target='.navbar-collapse'>
                        <span className='sr-only'>Toggle navigation</span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                    </button>
                </div>
                
                <div className='navbar-collapse collapse'>
                    <ul className='nav navbar-nav'>
                        <li>
                            <DisplayModal triggerElement={this.renderModalButton} onSubmit={ItemService.GetItem}/>
                        </li>
                        <li>
                            <AddModal triggerElement={this.renderModalButton} successCallback={()=>{alert("Record Created")}} onSubmit={ItemService.PostData}/>
                        </li>
                        <li>
                            <DeleteModal triggerElement={this.renderModalButton} successCallback={()=>{alert("Record Deleted")}} onSubmit={ItemService.DeleteItem}/>
                        </li>
                    </ul>
                </div>
            </div>
        )
    }
}
