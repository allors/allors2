export declare abstract class ErrorService {
    abstract message(error: Error): void;
    abstract dialog(error: Error): any;
}
