import { SessionObject, Method } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { Deletable } from './Deletable.g';
import { WorkItem } from './WorkItem.g';
import { Person } from './Person.g';
export interface Task extends SessionObject, UniquelyIdentifiable, Deletable {
    WorkItem: WorkItem;
    DateCreated: Date;
    DateClosed: Date;
    Participants: Person[];
    AddParticipant(value: Person): any;
    RemoveParticipant(value: Person): any;
    Performer: Person;
    CanExecuteDelete: boolean;
    Delete: Method;
}
