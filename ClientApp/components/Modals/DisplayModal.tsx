import * as React from 'react';
import {Button, Message, Modal,Input, Dimmer,Loader, Segment} from "semantic-ui-react";
import {Item} from "../../Services/ItemService";


export interface DisplayModalProps {
    onSubmit: (itemId: string)=> Promise<Response>;
    triggerElement: (name: string, clickFn:(e:React.SyntheticEvent<any>)=> void) => JSX.Element;
}

export interface DisplayModalState {
    itemId: string;
    item: Item | null;
    modalOpen: boolean;
    error: string;
    loading: boolean;
}

export default class DisplayModal extends React.Component<DisplayModalProps, DisplayModalState> {
    constructor() {
        super();
        this.state = { 
            modalOpen: false,
            loading: false,
            error: "",
            itemId: "",
            item: null
        };
    }

    public handleInputChange = (e : React.SyntheticEvent<any>, {name,value}: any) => {
        this.setState({itemId: value})
    }

   

    public canSubmit() : boolean{
        if(!this.state.itemId ||this.state.itemId.length != 10){
            return false;
        }
        return true;
    }

    private handleOpen = (e : React.SyntheticEvent<any>) => {
        e.stopPropagation();
        e.preventDefault();

        this.setState({ modalOpen: true });
    };

    private handleClose = () => this.setState( { 
        modalOpen: false,
        loading: false,
        error: "",
        itemId: "",
        item: null
    });

    private handleSubmission = () =>{
        this.setState({loading: true, item: null})
        this.props.onSubmit(this.state.itemId)
        .then(response =>{
            if(response.ok){
               (response.json() as Promise<Item>)
                .then(data => {
                    this.setState({ item: data, loading: false });
                });
            }
            else{
                if(response.status === 404){
                    this.setState({error: "Item not found."})
                }
                else{
                    this.setState({error: "Unable to display item. " + response.statusText})
                }
                this.setState({loading: false})
            }
        })
        .catch(ex=>{
            this.setState({error: "Unable to display item. "});
            this.setState({loading: false})
        });
    }

    private renderErrorMessage(){
        if(this.state.error != ""){
            return (<Message negative>{this.state.error}</Message>)
        }
        return ""
    }

    private renderItem(){
        if(this.state.item){
            return(
                <div>
                    <h3>Name:</h3>
                    <span>{this.state.item.Name}</span>
                    <h3>Phone Number:</h3>
                    <span>{this.state.item.PhoneNumber}</span>
                </div>
            )
        }
        return("")
    }

    public render() {
        return(
            <Modal className="scrolling" trigger={this.props.triggerElement("Display Item", this.handleOpen)} open={this.state.modalOpen} onClose={this.handleClose}>
                <Modal.Header>Delete an Item</Modal.Header>
                <Segment basic>
                        <Dimmer active={this.state.loading}>
                            <Loader />
                        </Dimmer>
                        {this.renderErrorMessage()}
                        {this.renderItem()}
                        <div className="FlexContent">
                            <Input value={this.state.itemId} onChange={this.handleInputChange} name="Item Id" label="Id"></Input>
                            <div>
                                <Button disabled={!this.canSubmit()} onClick={this.handleSubmission}>Submit</Button>
                                <Button onClick={this.handleClose}>Cancel</Button>
                            </div>
                        </div>
                </Segment>
            </Modal>)
    }
}
