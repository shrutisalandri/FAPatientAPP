import {
    Directive,
    EventEmitter,
    HostListener,
    Input,
    Output
    } from '@angular/core';

@Directive({
    selector: '[ggkScrollPosition]'
})
export class ScrollPositionDirective {

    @Input() public maxHeight: number;
    @Output() public scrollChange: EventEmitter<boolean> = new EventEmitter<boolean>();

    private _isScrolled: boolean;

    public ngOnInit(): void {
        this.onWindowScroll();
    }

    @HostListener('window:scroll')
    onWindowScroll(): void {
        let isScrolled = window.scrollY > this.maxHeight;
        if (isScrolled !== this._isScrolled) {
            this._isScrolled = isScrolled;
            this.scrollChange.emit(isScrolled);
        }
    }
}
