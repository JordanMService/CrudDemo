import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import AddModal from './Modals/AddModal';
import DeleteModal from './Modals/DeleteModal';
import DisplayModal from './Modals/DisplayModal';
import ItemService from "../Services/ItemService";
import { Button } from 'semantic-ui-react';

export class NavMenu extends React.Component<{}, {}> {

    private renderModalButton = (buttonText: string,clickFn: (e : React.SyntheticEvent<any>) => void) =>{
            return(<Button onClick={clickFn}>{buttonText}</Button>)
    } 

    public render() {
        return <div className='main-nav'>
                <div className='navbar navbar-inverse'>
                <div className='navbar-header'>
                    <button type='button' className='navbar-toggle' data-toggle='collapse' data-target='.navbar-collapse'>
                        <span className='sr-only'>Toggle navigation</span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                    </button>
                    <Link className='navbar-brand' to={ '/' }>Demo</Link>
                </div>
                <div className='clearfix'></div>
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
        </div>;
    }
}
